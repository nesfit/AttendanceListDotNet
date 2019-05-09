// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//$("#variantSelector").on("change", function() {
//	var item = $("#variantSelector option:selected").attr("value");

//	$.post("/Students",
//		{
//			variant: item
//		});
//})
$("#selectAll").click(function() {
	$("input[name='sIds']").prop("checked", this.checked);
});

$("#automaticRegButton").click(function(event) {
	var isChecked = $(this).prop("checked");
	$.post("autoregister", { toggle: isChecked ? 1 : 0 });
});

function checkedCheckboxes() {
	return $("input[type=checkbox]:checked").length;
}

function selectedInDropdown() {
	return $("#variant-selector").find(":selected").text() !== "";
}

function closeAlert(alertName) {
	$("#alert" + alertName).hide();
}

function submitStudentsForm(formAction) {
	if (checkedCheckboxes()) {
		$("#students-form").attr("action", formAction);
		$("#students-form").submit();
	}
	else {
		$("#alertNoStudentsSelected").show();
	}
}

function submitFilterForm(sortOrder) {
	$("input[name='sortOrder']").prop("value", sortOrder);
	$("#filter-form").submit();
}

function handleRegisterButton(isLoggedIn) {
	var i = 0;
	if (!isLoggedIn && ++i)
		$("#alertNotLoggedIn").show();
	if (!checkedCheckboxes() && ++i)
		$("#alertNoStudentsSelected").show();
	if (!selectedInDropdown() && ++i)
		$("#alertNoVariantSelected").show();
	if (!i) {
		$("#students-form").attr("action", "/Students/Register");
		$("#students-form").submit();
	}
}

function handleVariantButton(isLoggedIn) {
	if (!isLoggedIn)
		$("#alertNotLoggedIn").show();
	else {
		window.location.href = "/Variants/FindCourse";
	}
}

