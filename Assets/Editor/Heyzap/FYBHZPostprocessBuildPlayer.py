#!/usr/bin/python
import os
import sys
import re
from distutils import dir_util
from FYBHZmod_pbxproj import XcodeProject

def edit_pbxproj_file():
    try:
        print "FYBHZ: FYBHZPostProcessBuildPlayer started."
        unityProjectTopDirectory = sys.argv[1]
        pathToEmbeddedFrameworkResources = sys.argv[2]

        for xcodeproj in os.listdir(unityProjectTopDirectory):
            if not re.search('\.xcodeproj', xcodeproj):
                continue
            xcodeproj = os.path.join(unityProjectTopDirectory, xcodeproj)
            print "FYBHZ: found xcode proj: ", xcodeproj
            for pbxproj in os.listdir(xcodeproj):
                if not re.search('project\.pbxproj', pbxproj):
                    continue
                pbxproj = os.path.join(xcodeproj, pbxproj)
                print "FYBHZ: found pbxproj: ", pbxproj
                
                # locate the id of the "Frameworks" group of the pbxproj file so that frameworks will go to that group
                frameworksGroupID = None
                textfile = open(pbxproj, 'r')
                filetext = textfile.read()
                textfile.close()
                matches = re.findall("([0-9A-F]*) /\* FYBHZMediationTestSuiteEF.embeddedframework \*/ = \{\n\s*isa = PBXGroup;", filetext)
                try:
                    frameworksGroupID = matches[0]
                    print "FYBHZ: found embeddedframework group: ", frameworksGroupID
                except:
                    print "FYBHZ: did not find framework group."
                    pass

                print "FYBHZ: loading xcode project."
                project = XcodeProject.Load(pbxproj)
                print "FYBHZ: done loading xcode project. Adding resources."

                project.add_folder( pathToEmbeddedFrameworkResources, excludes=["^.*\.meta$"], parent=frameworksGroupID)

                project.save()
                print "FYBHZ: successfully modified file: ", pbxproj
                return 0
        raise FileExistsError("Could not find the 'project.pbxproj' file to edit")
    except Exception as e:
      print "FYBHZ: ERROR modifying 'project.pbxproj', error: ", e
      return 1

sys.exit(edit_pbxproj_file())
