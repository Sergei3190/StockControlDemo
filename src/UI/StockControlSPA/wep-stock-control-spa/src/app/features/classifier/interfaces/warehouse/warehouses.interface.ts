import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IWarehouseItem } from "./warehouse-item.interface";

export interface IWarehouses extends IPaginatedItems {
    items: IWarehouseItem[];
}