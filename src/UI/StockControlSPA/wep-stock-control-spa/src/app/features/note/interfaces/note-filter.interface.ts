import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface INoteFilter extends IFilter{
    executionDate?: any;
}