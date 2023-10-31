import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IStockAvailabilityItem } from "./stock-availability-item.interface";

export interface IStockAvailabilities extends IPaginatedItems {
    items: IStockAvailabilityItem[];
}