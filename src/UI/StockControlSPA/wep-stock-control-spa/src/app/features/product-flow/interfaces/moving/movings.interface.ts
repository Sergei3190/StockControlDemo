import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IMovingItem } from "./moving-item.interface";

export interface IMovings extends IPaginatedItems {
    items: IMovingItem[];
}