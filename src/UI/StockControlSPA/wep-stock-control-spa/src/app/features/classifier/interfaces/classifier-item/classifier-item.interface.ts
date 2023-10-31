import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";

export interface IClassifierItem extends INamedEntity{
    /** Тип справочника */
    classifier?: INamedEntity;
}
