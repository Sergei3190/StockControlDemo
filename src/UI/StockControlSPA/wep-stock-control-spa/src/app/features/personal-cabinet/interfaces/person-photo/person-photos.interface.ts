import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IPersonPhotoItem } from "./person-photo-item.interface";

export interface IPersonPhotos extends IPaginatedItems {
    items: IPersonPhotoItem[];
}