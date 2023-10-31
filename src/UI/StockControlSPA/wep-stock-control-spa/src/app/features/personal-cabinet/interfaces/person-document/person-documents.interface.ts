import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IPersonDocumentItem } from "./person-document-item.interface";

export interface IPersonDocuments extends IPaginatedItems {
    items: IPersonDocumentItem[];
}