module.exports = function(grunt) {
	grunt.initConfig({
		
		paths: {
			src: ['./src'],
			dist: ['./dist'],
			misc: ['./misc_projects']
		},

		clean: {
			options: {
				force: true
			},
			dist: ['<%= paths.dist %>/**/*']
		},

		browserify: {
			build: {
				src: ["<%=paths.src%>/Home/App.tsx"],
				dest: "<%=paths.dist%>/app.js",
				options: {
					browserifyOptions: {
						plugin: [
							['tsify']
						],
						watch: true
					}
				}
			},
			calendar: {
				src: ["<%=paths.src%>/Calendar/Calendar.tsx"],
				dest: "<%=paths.dist%>/calendar.js",
				options: {
					browserifyOptions: {
						plugin: [
							['tsify']
						],
						watch: true
					}
				}
			}
		},
		
		sass: {
			build: {
				options: {
					includePaths: [
						"<%=paths.src%>"
					]
				},
				files: {
					'<%=paths.dist%>/app.css': '<%=paths.src%>/home/app.scss',
					'<%=paths.dist%>/calendar.css': '<%=paths.src%>/calendar/calendar.scss',
				}
			}
		},

		copy: {
			images: {
				expand: true,
				cwd: "<%=paths.src%>/",
				src: ["bg.png", "favicon.ico"],
				dest: "<%=paths.dist%>/",
				flatten: true,
				filter: 'isFile'
			},
			markup: {
				expand: true,
				cwd: "<%=paths.src%>/",
				src: ["Home/App.html", "Calendar/Calendar.html"],
				dest: "<%=paths.dist%>/",
				flatten: true,
				filter: 'isFile'
			},
			misc: {
				expand: true,
				cwd: "<%=paths.misc%>/",
				src: ['**/*'],
				dest: "<%=paths.dist%>/",
				flatten: false
			}
		},

		watch: {
			images: {
				files: ['<%=paths.src%>/**/*.png'],
				tasks: ['copy:images'],
				options: {
					spawn: false,
				}
			},
			markup: {
				files: ['<%=paths.src%>/**/*.html'],
				tasks: ['copy:markup'],
				options: {
					spawn: false,
				}
			},
			styles: {
				files: ['<%=paths.src%>/**/*.scss'],
				tasks: ['sass:build'],
				options: {
					spawn: false,
				}
			},
			scripts: {
				files: ['<%=paths.src%>/**/*.ts', '<%=paths.src%>/**/*.tsx'],
				tasks: ['browserify:build'],
				options: {
					spawn: false,
				}
			}
		},

	});

	grunt.loadNpmTasks('grunt-browserify');
	grunt.loadNpmTasks('grunt-sass');
	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-contrib-watch');

	grunt.registerTask('build', ['clean', 'browserify', 'sass', 'copy']);
	grunt.registerTask('default', ['build']);
}