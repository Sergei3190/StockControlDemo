import { IFilter } from "src/app/shared/interfaces/filter.interface";

export interface IUserPersonFilter extends IFilter{
    age?: number;
    birthday?: any;
}