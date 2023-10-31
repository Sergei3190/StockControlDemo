import { Guid } from "guid-ts";
import { IProductFilter } from "src/app/shared/interfaces/product-filter.interface";

export interface ISendingProductFilter extends IProductFilter {
    sendingWarehouseId?: Guid;
}