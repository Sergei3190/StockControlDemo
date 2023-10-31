import { IEntity } from "src/app/shared/interfaces/entity.interface";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";

export interface INotificationSettingItem extends IEntity{
    /** Тип уведомления */
    notificationType: INamedEntity;

    /** Признак включенного уведомления */
    enable: boolean;
}