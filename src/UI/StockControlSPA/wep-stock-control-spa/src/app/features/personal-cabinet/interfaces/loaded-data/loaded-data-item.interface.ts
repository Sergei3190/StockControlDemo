import { Guid } from "guid-ts";
import { IEntity } from "src/app/shared/interfaces/entity.interface";
import { IFileInfo } from "src/app/shared/interfaces/file-info.interface";
import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";

export interface ILoadedDataItem extends IEntity{
    /** Идентификатор карты персоны */
    cardId: Guid;

    /** Загруженный пользователем файл */
    file: IFileInfo;

    /** Тип загруженных данных */
    loadedDataType?: INamedEntity;
}