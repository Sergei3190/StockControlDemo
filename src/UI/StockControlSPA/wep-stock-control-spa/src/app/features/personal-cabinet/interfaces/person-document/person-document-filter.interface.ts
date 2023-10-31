import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface IPersonDocumentFilter extends IFilter{
    cardId: Guid;
}