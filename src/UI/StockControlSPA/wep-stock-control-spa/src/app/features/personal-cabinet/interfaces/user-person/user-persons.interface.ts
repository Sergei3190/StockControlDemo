import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IUserPersonItem } from "./user-person-item.interface";

export interface IUserPersons extends IPaginatedItems {
    items: IUserPersonItem[];
}