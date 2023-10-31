import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";

export interface IClassifier extends INamedEntity{    
    //** Путь к справочнику */
    path: string;
}
