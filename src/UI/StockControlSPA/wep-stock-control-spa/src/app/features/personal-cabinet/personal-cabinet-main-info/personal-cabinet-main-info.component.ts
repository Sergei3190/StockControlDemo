import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, debounceTime, take, takeUntil } from 'rxjs';
import { UserPersonsService } from './services/user-persons.service';
import { ToastrService } from 'ngx-toastr';
import { IUserPersonItem } from '../interfaces/user-person/user-person-item.interface';

@Component({
  selector: 'app-personal-cabinet-main-info',
  templateUrl: './personal-cabinet-main-info.component.html',
  styleUrls: ['./personal-cabinet-main-info.component.scss']
})
export class PersonalCabinetMainInfoComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  @Input()
  model: IUserPersonItem;

  form: FormGroup;

  constructor(private fb: FormBuilder,
    private service: UserPersonsService,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  clearField(field: string): void {
    this.form.get(field)?.setValue('');
  }

  getErrorMessage(name: string): string {
    switch (name) {
      case 'lastName':
        return this.form.get(name)?.hasError('required') ? 'Поле "Фамилия" обязательно' : '';
      case 'firstName':
        return this.form.get(name)?.hasError('required') ? 'Поле "Имя" обязательно' : '';
      default: return '';
    }
  }

  private initProperties() {
    if (!this.model){
      this.model = {} as IUserPersonItem;
    }
    this.form = this.fb.group({
      id: [this.model.id],
      lastName: [this.model.lastName, Validators.required],
      firstName: [this.model.firstName, Validators.required],
      middleName: [this.model.middleName],
      age: [this.model.age],
      birthday: [this.model.birthday],
      cardId: [this.model.cardId]
    });
  }

  private initSubscriptions() {
    this.form.valueChanges
      .pipe(
        debounceTime(300), // чтобы при нажатии клавиш пользователем несколько раз подряд, сохранялось актуальное значение
        takeUntil(this.destroy$))
      .subscribe(values => {
        if (this.form.valid && this.form.dirty) {

          const message = this.toastr.success(
            '<a class="restore-form-action text-decoration-underline">Вернуть</a> предыдущее значение',
            'Заметка изменена'
          );

          let action!: HTMLElement;

          message.onShown
            .pipe(take(1))
            .subscribe(() => {
              action = document.querySelector('.restore-form-action')!;
              if (action) {
                  action.addEventListener('click', () => this.restoreForm());
              }
          });

          message.onHidden.pipe(take(1)).subscribe(() => {
            if (action) {
                action.removeEventListener('click', () => this.restoreForm());
            }
            this.applyChanges({...values});
          });
        }
      });
  }

  private restoreForm(): void {
    this.form.reset(this.model);
  }

  private applyChanges(value: any) {
    if (!this.form.dirty) {
      return;
    }
    this.model = Object.assign(this.model, value);
    this.checkValue(this.model);
    this.service.updateUserPerson(this.model)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(flag => {
        if (flag){
          this.toastr.success('Данные успешно изменены');
        }
      });
  }

  private checkValue(model: IUserPersonItem) {
    if (model.birthday === ""){
      model.birthday = null;
    }
  }
}


