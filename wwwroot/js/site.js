// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    function deleteGoal(goalArr) {
        $.ajax({
            datatype: "JSON",
            method: "POST",
            url: "DeleteGoal",
            data: {
                "GoalLabel": goalArr[1]
            },
            success: function (data) {
                console.log("This is the data: " + data);
                $("#" + goalArr[0]).html(data);
            }
        })
    }

    $(document).on("click", ".delete-button", function (event) {

        event.preventDefault();

        // 0'th element is the Id number, and the 1st is the Label
        var goalArr = $(this).val().split(", ");
        console.log("You pressed the (" + goalArr[1] + ") delete button :p");
        deleteGoal(goalArr);
    });


    function addGoal() {
        $.ajax({
            datatype: "JSON",
            method: "POST",
            url: "AddGoal",
            success: function (data) {
                console.log("Making new goal... of: " + data);
                $("#Create").html(data);
            }
        })
    }

    $(document).on("click", ".add-button", function (event) {

        event.preventDefault();

        console.log("You pressed the create button :p");
        addGoal();
    });

})