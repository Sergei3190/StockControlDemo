import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";

export interface IPagination extends IPaginatedItems{
    items: number;
}