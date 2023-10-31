import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IOrganizationItem } from "./organization-item.interface";

export interface IOrganizations extends IPaginatedItems {
    items: IOrganizationItem[];
}