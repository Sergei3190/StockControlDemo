import { IEntity } from "src/app/shared/interfaces/entity.interface";

export interface IFileInfo extends IEntity{
    /** Имя файла */
    fileName?: string;

    /** Тип содержимого */
    contentType?: string;

    /** Длина содержимого */
    contentLength?: any;

    /** содержимое в байтах */
    content?: Blob;
}