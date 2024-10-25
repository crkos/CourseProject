
export class LikeButton {
    /**
     * @type {JQuery<HTMLButtonElement>}
     */
    _buttonLike

    /**
     * @param {JQuery<HTMLButtonElement>} buttonRefs
     */
    constructor(buttonRefs) {
        this._buttonLike = buttonRefs

    }
}

export class LikeController {
    /**
     * @type {LikeButton[]}
     * */
    _likes;

    /**
     * 
     * @param {LikeButton[]} likes
     */
    constructor(likes) {
        this._likes = likes;

    }

    init() {
        this._likes.forEach(like => {
            like._buttonLike.on("click", this.getLikeId.bind(this))
        })
    }

    getLikeId(event, call) {
        let clickedElement = $(event.target);

        // Traverse up the DOM tree until we find the button or reach the top
        while (!clickedElement.is('button') && clickedElement.length > 0) {
            clickedElement = clickedElement.parent();
        }

        if (clickedElement.is('button')) {
            call(clickedElement.val());
        } else {
            console.log("No button found");
        }
    }
}