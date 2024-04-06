.PHONY: package
package:
	DOCKER_BUILDKIT=1 docker build  \
		--tag webapi:latest .

.PHONY: run-container
run-container: package
	docker run -p 8080:8080 --rm -it --name webapi webapi:latest
