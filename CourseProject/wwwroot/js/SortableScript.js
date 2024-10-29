const el = document.getElementById('simpleList');
const templateIdUrl = window.location.pathname.split('/')[2];
// Simple list
Sortable.create(el, {
    animation: 150,
    handle: '.handle',
	onEnd: function (evt) {
		const itemEl = evt.item;
		const oldOrder = evt.oldIndex + 1;
		const newOrder = evt.newIndex + 1;

		if (oldOrder === newOrder) {
			return;
		}

		const inputWithQuestionId = itemEl.querySelector('input[name*="QuestionId"]');
		const jquerySel = $(inputWithQuestionId).val();


		$.ajax({
			url: `/template/order`,
			contentType: "application/json",
			type: "PATCH",
			data: JSON.stringify({
				TemplateId: Number(templateIdUrl),
				QuestionId: Number(jquerySel),
				OldOrder: oldOrder ,
				NewOrder: newOrder
			}),
			success: () => {
				toastr.success("Updated question order!");
			},
			error: () => {
				toastr.error("Something happened, try again!");
			}
		})
	},
});