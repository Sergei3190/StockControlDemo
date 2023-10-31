import { Guid } from "guid-ts";
import { INoteItem } from "../interfaces/note-item.interface";

export class NoteItem implements INoteItem{
    id: Guid;

    /** Текст заметки */
    content: string;

    /** Признак избранной заметки */
    isFix: boolean;

    /** Номер сортировки */
    sort: number;

    /** Дата выполнения заметки */
    executionDate?: any;

    //** Признак отображения действий с заметкой */
    isHover: boolean;

    constructor(id: Guid = Guid.empty(), content: string, isFix = false, sort = -1, executionDate?: Date){
        this.id = id;
        this.content = content;
        this.isFix = isFix;
        this.sort = sort;
        this.executionDate = executionDate;
    }
}