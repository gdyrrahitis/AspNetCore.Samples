/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var paths = {
    nodeModules: "node_modules/",
    destination: "./wwwroot/libs/"
}

gulp.task('copy-boostrap', function () {
    return gulp.src(paths.nodeModules + "bootstrap/**/*.*")
        .pipe(gulp.dest(paths.destination + "bootstrap/"));
});

gulp.task('copy-jquery', function () {
    return gulp.src(paths.nodeModules + "jquery/**/*.*")
    .pipe(gulp.dest(paths.destination + "jquery/"));
});

gulp.task('copy-font-awesome', function () {
    return gulp.src(paths.nodeModules + "font-awesome/**/*.*")
    .pipe(gulp.dest(paths.destination + "font-awesome/"));
});

gulp.task("default:copy", ["copy-boostrap", "copy-jquery", "copy-font-awesome"]);