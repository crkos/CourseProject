import { TemplateCard, TemplateController } from './Templates.js'
import { LikeButton, LikeController } from './Likes.js';
import { Question } from "./Question.js";

const ref = $('.createTemplateForm')
const likeButton = new LikeButton($('.likeButton'));
const questionsContainer = $(".questionsContainer");
const addQuestionBtn = $(".addQuestion");
const templateViewButton = $(".viewUserTemplate").toArray();


const values = ref.toArray().map(ele => new TemplateCard($(ele)));

const templateController = new TemplateController(values, questionsContainer);



// Context of course who would've thought...
likeButton._buttonLike.on("click", templateController.likeTemplate.bind(templateController));
addQuestionBtn.on("click", templateController.addNewQuestion.bind(templateController));

templateViewButton.forEach(button => {
    $(button).on("click", function () {
        const value = $(this).val();

        // Redirect to the desired URL with the dynamic value
        window.location.href = `/template/${value}/edit`;
    });
});

templateController.init()


const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

/*!
 * Color mode toggler for Bootstrap's docs (https://getbootstrap.com/)
 * Copyright 2011-2024 The Bootstrap Authors
 * Licensed under the Creative Commons Attribution 3.0 Unported License.
 */

(() => {
  'use strict'

   const getStoredTheme = () => localStorage.getItem('theme')
   const setStoredTheme = theme => localStorage.setItem('theme', theme)

  const getPreferredTheme = () => {
    const storedTheme = getStoredTheme()
    if (storedTheme) {
      return storedTheme
    }

    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
  }

  const setTheme = theme => {
    if (theme === 'auto') {
      document.documentElement.setAttribute('data-bs-theme', (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'))
    } else {
      document.documentElement.setAttribute('data-bs-theme', theme)
    }
  }

  setTheme(getPreferredTheme())

  const showActiveTheme = (theme, focus = false) => {
    const themeSwitcher = document.querySelector('#bd-theme')

    if (!themeSwitcher) {
      return
    }

    const themeSwitcherText = document.querySelector('#bd-theme-text')
    const activeThemeIcon = document.querySelector('.theme-icon-active use')
    const btnToActive = document.querySelector(`[data-bs-theme-value="${theme}"]`)
    const svgOfActiveBtn = btnToActive.querySelector('svg use').getAttribute('href')


    document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
      element.classList.remove('active')
      element.setAttribute('aria-pressed', 'false')
    })

    btnToActive.classList.add('active')
    btnToActive.setAttribute('aria-pressed', 'true')
    activeThemeIcon.setAttribute('href', svgOfActiveBtn)
    const themeSwitcherLabel = `${themeSwitcherText.textContent} (${btnToActive.dataset.bsThemeValue})`
    themeSwitcher.setAttribute('aria-label', themeSwitcherLabel)

    if (focus) {
      themeSwitcher.focus()
    }
  }

  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
    const storedTheme = getStoredTheme()
    if (storedTheme !== 'light' && storedTheme !== 'dark') {
      setTheme(getPreferredTheme())
    }
  })

  window.addEventListener('DOMContentLoaded', () => {
    showActiveTheme(getPreferredTheme())

    document.querySelectorAll('[data-bs-theme-value]')
      .forEach(toggle => {
        toggle.addEventListener('click', () => {
          const theme = toggle.getAttribute('data-bs-theme-value')
          setStoredTheme(theme)
          setTheme(theme)
          showActiveTheme(theme, true)
        })
      })
  })
})()

$(document).ready(function () {
    const currentTemplateId = templateController.getTemplateIdPath();
    // Connect to the SignalR hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/commentsHub")
        .build();

    // Start the connection
    connection.start().then(function () {
        console.log("Connected to SignalR hub");
    }).catch(function (err) {
        console.error(err.toString());
    });

    // Function to handle received comments
    connection.on("ReceiveComment", function (templateId, commentText, commentedBy, createdDate) {
        // Check if the comment is for the current template
        if (templateId === Number(currentTemplateId)) {
            const newCommentHtml = `
                                <div class="comment-item mb-4">
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div>
                                        <strong>${commentedBy}</strong>
                                        <small class="text-muted">(${createdDate})</small>
                                         </div>
                                    </div>
                                    <div class="mt-2">
                                    <p>${commentText}</p>
                                    </div>
                                </div>
                            `;

            // Append the comment to the comment section
            $(".userComments").append(newCommentHtml);
        }
    });
});

document.getElementById('commentForm').addEventListener('submit', async function (event) {
    // Prevent the default form submission behavior
    event.preventDefault();

    // Get the form data
    const formData = new FormData(this);

    try {
        // Send the form data using Fetch API
        const response = await fetch('/Template/AddComment', {
            method: 'POST',
            body: formData
        });

        toastr.success("Posted comment!");
    } catch (error) {
        console.error('Error:', error);
    }
});