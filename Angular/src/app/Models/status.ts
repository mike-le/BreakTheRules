import { Idea } from './idea';

export class Status 
{
    statusId: number;
    statusCode: number;
    response: string;
    submitDt: Date;
    ideaId: number;
    idea: Idea;
}