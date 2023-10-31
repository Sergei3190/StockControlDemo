import { Guid } from "guid-ts";
import { IEntity } from "src/app/shared/interfaces/entity.interface";

export interface IUserPersonItem extends IEntity{
    /** Фамилия */
    lastName: string;

    /** Имя */
    firstName: string;

    /** Отчество */
    middleName?: string;

    /** Возраст */
    age?: number;

    /** День рождения */
    birthday?: any;

    /** Идентификатор карты персоны */
    cardId: Guid;
}