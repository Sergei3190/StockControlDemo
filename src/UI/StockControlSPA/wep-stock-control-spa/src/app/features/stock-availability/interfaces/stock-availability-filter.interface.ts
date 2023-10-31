import { IFilter } from "src/app/shared/interfaces/filter.interface";
import { IOrderByFilter } from "src/app/shared/interfaces/order-by-filter.interface";
import { IProductFilter } from "src/app/shared/interfaces/product-filter.interface";

export interface IStockAvailabilityFilter extends IFilter, IProductFilter, IOrderByFilter {
    
}