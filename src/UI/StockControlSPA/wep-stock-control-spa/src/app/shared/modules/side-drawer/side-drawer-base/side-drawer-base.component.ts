import { Component, OnDestroy, Type, Output, Input } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-side-drawer-base',
  template: ``
})

export abstract class SideDrawerBaseComponent<T = any> implements OnDestroy {
  protected readonly destroy$ = new Subject<void>();

  @Input()
  type: Type<T>;

  // здесь можно передать из вне данные для инициализации компонента
  @Input()
  data: T;

  // компонент должен иметь действие закрытия выдвижного ящика
  @Input()
  close:() => void;

  // будем при инициализации передавать стрим, тем самым свяжем компонент внешний и внутренний, чтобы можно было делать подписку
  @Input()
  sideDrawerDataChange: Subject<T>;

  ngOnDestroy(): void {
    // говорим, что значений для подписантов нет, завершаем работу
    this.destroy$.next();
    this.destroy$.complete();
  }
}
