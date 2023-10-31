import { IFileInfo } from "../interfaces/file-info.interface";
import { Guid } from "guid-ts";

export class FileInfoModel implements IFileInfo{
    id: Guid;

    /** Имя файла */
    fileName?: string;

    /** Тип содержимого */
    contentType?: string;

    /** Длина содержимого */
    contentLength?: any;

    /** содержимое в байтах */
    content?: Blob;
}