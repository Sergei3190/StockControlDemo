import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IWriteOffItem } from "./write-off-item.interface";

export interface IWriteOffs extends IPaginatedItems {
    items: IWriteOffItem[];
}