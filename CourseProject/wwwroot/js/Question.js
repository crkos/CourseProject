export class Question {
    /**
     * @type 
     */
    title = 'Default';
    type = 'TEXT';
    visible = true;
    order = 1;

    constructor(title, type, visible, order, ref) {
        this.title = title;
        this.type = type;
        this.visible = visible;
        this.order = order;
    }

}