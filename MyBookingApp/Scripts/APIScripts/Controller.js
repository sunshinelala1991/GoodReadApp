/*app.controller('APIController', function ($scope, APIService) {
    getAll(); 
    function getAll() {
        var servCall = APIService.getBooks();
        servCall.then(function (d) {
            $scope.books = d.data;
        }, function (error) {
                $log.error('Oops! Something went wrong while fetching the data.')
            })
    }

    $scope.clickBook=function(id) {
        APIService.getBookDetail(id).then(function (d) {
            console.log(d.data);
        });
    }

})  */


app.controller('APIController', function ($scope, $http) {

    $scope.formData = {};

    $http.get('/api/books').then(function (d) {
        $scope.books = d.data;
    }, function (error) {
        console.log('error ' + error);
    });




    $scope.getBookDetail = function (id) {
        $http.get('/api/Books/' + id).then(function (d) {
            console.log(d.data);
        }, function (data) {
            console.log('error ' + data);
        });
    }

    $scope.goToCreatePage = function () {
        location.href = "/books/create";
    };

    $scope.createNewBook = function () {
        $http.post('/api/Books', $scope.formData).then(function (d) {
            console.log(d.data);
            location.href = "/books";
        }, function (error) {
            console.log('error' + error);
        });
    }



    $scope.goToEditPage = function (id) {
        location.href = "/books/edit/"+id;
    }


    $scope.deleteBook = function (id) {
        $http.delete('/api/Books/' + id).then(function (d) {
            $http.get('/api/books').then(function (d) {
                $scope.books = d.data;
            }, function (error) {
                console.log('error ' + error);
            });
        }, function (error) {
            console.log('error' + error);
        });
    };



    angular.element(document).ready(function () {
        var matchedAuthors = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '../api/authors?query=%QUERY',
                wildcard: '%QUERY'
            }
        });

        $('#bookAuthor').typeahead({ minLength: 1, highlight: true }, {
            name: 'authors',
            display: 'name',
            source: matchedAuthors,

            templates: {
                empty: function (context) {
                    
                    $scope.formData.AuthorId = 0;
                }
            }


        }).on("typeahead:select", function (e, author) {
            $scope.formData.AuthorId = author.id;
            console.log(author.id);
        });
    });

});


app.controller('AuthorsController', function ($scope, $http) {
    $http.get('/api/authors').then(function (d) {
        $scope.authors = d.data;
    }, function (error) {
        console.log('error' + error);
        });


});


app.controller('EditBookController', function ($scope, $http) {
    $scope.editFormData = {};

    var bookId = location.href.substr(location.href.lastIndexOf('/') + 1);
    $http.get('/api/books/' + bookId).then(function (d) {
        $scope.editFormData = d.data;
    }, function (error) {
        console.log('error ' + error);
        });



    $scope.editBook = function () {
        $http.put('/api/Books/' + bookId, $scope.editFormData).then(function (d) {
            console.log(d.data);
            location.href = "/books";
        }, function (error) {
            console.log('error' + error);
        });
    }





    angular.element(document).ready(function () {
        var matchedAuthors = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/api/authors?query=%QUERY',
                wildcard: '%QUERY'
            }
        });

        $('#bookAuthor').typeahead({ minLength: 1, highlight: true }, {
            name: 'authors',
            display: 'name',
            source: matchedAuthors,

            templates: {
                empty: function (context) {

                    $scope.editFormData.AuthorId = 0;
                }
            }


        }).on("typeahead:select", function (e, author) {
            $scope.editFormData.AuthorId = author.id;
            console.log(author.id);
        });
    });



});
