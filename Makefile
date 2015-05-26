# Builds all the projects in the solution...
.PHONY: all_projects
all_projects: CloneCli 

# Builds project 'CloneCli'...
.PHONY: CloneCli
CloneCli: 
	$(MAKE) --directory="CloneCli/" --file=CloneCli.makefile

# Cleans all projects...
.PHONY: clean
clean:
	make --directory="CloneCli/" --file=CloneCli.makefile clean

