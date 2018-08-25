import { Idea } from './idea';

export class Theme {
    themeId: number;
    title: string;
    description: string;
    openDt: Date;
    closeDt: Date;
    owner: string;
    status: string;
    ideas: Idea[];
}