import { IEntity } from "src/app/shared/interfaces/entity.interface";

export interface INoteItem extends IEntity{
    /** Текст заметки */
    content: string;

    /** Признак избранной заметки */
    isFix: boolean;

    /** Номер сортировки */
    sort: number;

    /** Дата выполнения заметки */
    executionDate?: any;
}