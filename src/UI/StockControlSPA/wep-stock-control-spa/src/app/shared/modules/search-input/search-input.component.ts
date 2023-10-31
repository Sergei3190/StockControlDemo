import { Component, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject, Observable, takeUntil } from 'rxjs';

@Component({
  selector: 'app-search-input',
  templateUrl: './search-input.component.html',
  styleUrls: ['./search-input.component.scss']
})
export class SearchInputComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  
  @Input()
  canClear: Observable<boolean>;

  @Output()
  searchChange = new Subject<string>();

  quickSearchText?: string;
  previosValue?: string;

  constructor(){
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
  }

  ngOnDestroy(): void {
    // говорим, что значений для подписантов нет, завершаем работу
    this.destroy$.next();
    this.destroy$.complete();
  }

  onChangedInput(event: any): void {
    if (event === this.previosValue){
      return;
    }
    setTimeout(() => this.searchChange.next(event), 500); // 500ms, чтобы при нажатии клавиш пользователем несколько раз подряд, сохранялось актуальное значение
    this.previosValue = event;
  }

  private initProperties() {
   this.quickSearchText = '';
    this.previosValue = '';
  }

  private initSubscriptions() {
    this.canClear
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(flag => flag ? this.initProperties() : '');
  }
}
