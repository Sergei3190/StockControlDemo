import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { IClassifier } from "./classifier.interface";

export interface IClassifiers extends IPaginatedItems {
    items: IClassifier[];
}