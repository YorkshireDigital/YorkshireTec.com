module.exports = function(grunt) {
	// Project configuration.
	grunt.initConfig({

		// Grunticon setup
		grunticon: {
			myIcons: {
					files: [{
						expand: true,
						cwd: 'public/styles/source',
			            src: ['*.svg', '*.png'],
			            dest: "public/styles"
					}],
				options: {
					
					// Set output path for the PNG's in the fallback stylesheet
					pngpath: "/img",

					// prefix for CSS classnames - icn is already being prefixed when exporting from AI. Replace this with a grunt task going forward.
					cssprefix: ".",

					cssbasepath: "css",

					// JS loader filename & path
					loadersnippet: "../scripts/grunticon.loader.js",

					// CSS file & path names
					datasvgcss: "/css/icons.data.svg.css",
					datapngcss:	"/css/icons.data.png.css",
					urlpngcss: 	"/css/icons.fallback.css",

					// Preview HTML filename
					previewhtml: "/source/preview.html",

				},
				clean: {
					tests: ['tmp']
				}
			}
		},

		// Watch tasks
		watch: {
			svgWatch: {
				files: ['public/styles/source/*.svg'],
				tasks: ['grunticon']
			}
		},

		// Rename files
		rename: {
			imageRename: {

			}
		}

	});
	grunt.loadNpmTasks('grunt-grunticon');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-rename');

	grunt.registerTask('default', ['grunticon:myIcons']);
	grunt.registerTask('test', ['grunt-contrib-watch']);
	grunt.registerTask('test2', ['grunt-rename']);
	
};