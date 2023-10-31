import { IPaginatedItems } from "src/app/shared/interfaces/paginated-items.interface";
import { NoteItem } from "../models/note-item.model";

export interface INotes extends IPaginatedItems {
    items: NoteItem[];
}