import { Question } from './Question.js';

export class TemplateCard {
    /**
     * @type {JQuery<HTMLButtonElement>}
     */
    _buttonCard

    /**
     * @param {JQuery<HTMLButtonElement>} buttonRefs
     */
    constructor(buttonRefs) {
        this._buttonCard = buttonRefs
    }
}

export class TemplateController {
    /**
     * @type {TemplateCard[]}
     */
    _templates;
    /**
     * @type {JQuery<HTMLElement>}
     */
    _questionContainer;
   

    /**
     * 
     * @param {TemplateCard[]} templates
     * @param {JQuery<HTMLElement>} questionContainer
     */
    constructor(templates, questionContainer) {
        this._templates = templates;
        this._questionContainer = questionContainer;
    }

    init() {
        this._templates.forEach(template => {
            template._buttonCard.on("click", this.getTemplateId.bind(this))
        })
    }

    /**
     * Get the template id to clone it.
     * @param {Event} event
     * @returns {void}
     */
    getTemplateId(event) {
        let clickedElement = $(event.target);

        // Traverse up the DOM tree until we find the button or reach the top
        while (!clickedElement.is('button') && clickedElement.length > 0) {
            clickedElement = clickedElement.parent();
        }

        if (clickedElement.is('button')) {
            this.cloneTemplate(clickedElement.val());
        } else {
            console.log("No button found");
        }
    }

    addNewQuestion() {
        const templateId = this.getTemplateIdPath();
        $.ajax({
            url: `/Template/AddQuestion`,
            contentType: "application/json",
            type: "POST",
            data: JSON.stringify(templateId),
            success: ({ id, questionText, questionType, isVisible, order }) => {

                // Get the current number of questions to determine the new index
                const questionCount = this._questionContainer.children().length;
                const newIndex = questionCount; // The next index for the new question

                // Use the newIndex for naming the new question's form inputs
                const newQuestionHtml = `
            <div class="container-fluid bg-body-tertiary p-4 shadow rounded-3 mt-4 question">
                <input type="hidden" name="Questions[${newIndex}].QuestionId" value="${id}" />
                <input type="hidden" name="Questions[${newIndex}].QuestionType" value="${questionType}" />
                <div class="d-flex justify-content-center align-items-center">
                    <i class="bi bi-grip-horizontal text-center fs-1 handle" role="button" title="Move element"></i>
                </div>
                <label for="Questions[${newIndex}].QuestionText" class="form-label">Question</label>
                <input name="Questions[${newIndex}].QuestionText" value="${questionText}" class="form-control w-50 fs-2 mb-4" />
                <input placeholder="Short Answer" class="form-control w-50 border-0" disabled />
            </div>
            `;

                // Append the new question to the question container
                this._questionContainer.append(newQuestionHtml);
            },
            error: (error) => {
                if (error.status === 401) {
                    toastr.warning('<a href="/">Sign in/Sign up to like this template!</a>');
                } else {
                    toastr.warning("There was an error while adding a new question, try again later.");
                }
            }
        });
    }


    /**
     * Likes the current template
     */
    likeTemplate() {
        const templateId = this.getTemplateIdPath();
        const likeIcon = $('.likeIcon')
        const likeCounter = $('.likeCounter');
        $.ajax({
            url: `/Template/${templateId}/Like`,
            type: "POST",
            success: ({message, liked}) => {
                toastr.success(message)

                if (liked) {
                    likeIcon.removeClass('bi-heart').addClass('bi-heart-fill');
                    likeCounter.text(parseInt(likeCounter.text()) + 1);
                } else {
                    likeIcon.removeClass('bi-heart-fill').addClass('bi-heart');
                    likeCounter.text(parseInt(likeCounter.text()) - 1);
                }
            },
            error: (error) => {

                if (error.status === 401) {
                    toastr.warning('<a href="/">Sign in/Sign up to like this template!</a>');
                } else {
                    toastr.warning("There was an error while liking/disliking this template, please try again later");
                }

                
            } 
        })
    }

    /**
     * 
     * Clone the template id from the user dashboard
     * @param {number} templateId
     */
    cloneTemplate(templateId) {
        $.ajax({
            url: "/Template/Clone",
            contentType: "application/json",
            type: "POST",
            data: JSON.stringify(templateId),
            success: ({ value }) => {
                if (value.id) {
                    window.location.href = `/template/${value.id}/edit`;
                }
            },
            error: () => {
                toastr.warning("There was an error while cloning this template, please try again later!")
            }
        })
    }

    /**
     * 
     * Returns the current template id from the URL
     * @returns {number}
     */
    getTemplateIdPath() {
        return window.location.pathname.split('/')[2];
    }
}
