import { IEntity } from "src/app/shared/interfaces/entity.interface";

export interface IParty extends IEntity {
    //** Номер партии изготовителя */
    number?: string;

    //** Уникальный номер партии получателя  */
    extensionNumber?: string;

    //** Дата создания партии */
    createDate?: any;

    //** Время создания партии */
    createTime?: any;
}