import { INamedEntity } from "src/app/shared/interfaces/named-entity.interface";

export interface IProductFlowInfo {
    /** Тип движения (поступление/перемещение/списание) */
    productFlowType?: INamedEntity;

    /** Номер поступления/перемещения/списания */
    number: string;

    /** Дата поступления/перемещения/списания */
    createDate: any;

    /** Время поступления/перемещения/списания */
    createTime: any;
}