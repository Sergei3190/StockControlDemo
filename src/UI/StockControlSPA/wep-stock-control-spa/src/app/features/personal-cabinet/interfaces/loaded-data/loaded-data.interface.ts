import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { ILoadedDataItem } from "./loaded-data-item.interface";

export interface ILoadedData extends IPaginatedItems {
    items: ILoadedDataItem[];
}