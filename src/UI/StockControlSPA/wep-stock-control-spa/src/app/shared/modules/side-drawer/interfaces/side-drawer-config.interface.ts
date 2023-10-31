import { Type } from "@angular/core";
import { SideDrawerBaseComponent } from "../side-drawer-base/side-drawer-base.component";
import { Subject } from "rxjs";

/** Интерфейс для создания компонента выдвижного ящика */
export interface ISideDrawerConfig<T extends SideDrawerBaseComponent>{
    type: Type<T>;
    // здесь можно инициализировать данные компонента
    data: T;
    // компонент должен иметь действие закрытия выдвижного ящика
    close:() => void;
    // будем при инициализации передавать стрим, тем самым свяжем компонент внешний и внутренний, чтобы можно было делать подписку
    sideDrawerDataChange?: Subject<T>;
}