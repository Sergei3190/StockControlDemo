import { Guid } from "guid-ts";
import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface IClassifierItemFilter extends IFilter{
    classifierId?: Guid;
}