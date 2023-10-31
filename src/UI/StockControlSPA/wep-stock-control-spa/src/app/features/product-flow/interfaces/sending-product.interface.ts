import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";
import { IProduct } from "src/app/shared/interfaces/product.interface";

export interface ISendingProduct extends IProduct {
    /** Склад отправитель */
    sendingWarehouse?: INamedEntity;
}