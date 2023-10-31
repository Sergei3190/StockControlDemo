import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IReceiptItem } from "./receipt-item.interface";

export interface IReceipts extends IPaginatedItems {
    items: IReceiptItem[];
}