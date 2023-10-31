import { INamedEntity } from "./named-entity.interface";
import { IParty } from "./party.interface";

export interface IProduct {
    /** Партия */
    party: IParty;

    /** Номенклатура */
    nomenclature: INamedEntity;

    /** Склад хранения/получатель */
    warehouse: INamedEntity;

    /** Организация (поставщик) */
    organization: INamedEntity;

    /** Цена за единицу товара */
    price: string;

    /** Количество товара */
    quantity: number;

    /** Итоговая цена */
    totalPrice?: string;
}