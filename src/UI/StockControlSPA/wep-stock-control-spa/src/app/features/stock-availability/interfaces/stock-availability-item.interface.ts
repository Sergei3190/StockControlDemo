import { Guid } from "guid-ts";
import { IEntity } from "src/app/shared/interfaces/entity.interface";
import { IProduct } from "src/app/shared/interfaces/product.interface";

export interface IStockAvailabilityItem extends IEntity, IProduct {
    //** Поступление */
    receiptId?: Guid;

    //** Перемещение */
    movingId?: Guid;

    //** Списание */
    writeOffId?: Guid;
}