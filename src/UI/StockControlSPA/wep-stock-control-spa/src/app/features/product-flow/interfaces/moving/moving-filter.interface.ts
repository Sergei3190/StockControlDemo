import { IFilter } from "src/app/shared/interfaces/filter.interface";
import { IOrderByFilter } from "src/app/shared/interfaces/order-by-filter.interface";
import { ISendingProductFilter } from "../sending-product-filter.interface";

export interface IMovingFilter extends IFilter, ISendingProductFilter, IOrderByFilter {
    
}