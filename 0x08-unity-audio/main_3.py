import yaml

def removeUnityTagAlias(filepath):
    """ removes unnecessary Unity tags and adds ID to node"""
    result = str()

    with open(filepath) as srcFile:
        for lineNumber,line in enumerate(srcFile.readlines()): 
            if line.startswith('--- !u!'):          
                result += '\n--- ' + line.split(' ')[2]   # remove the tag, but keep file ID
                result += '\nID: ' + line.split('&')[1]   # add file ID
            else:
                result += line

    return (result)


def loadYAML(filepath):
    """ loads nodes from YAML and appends to list """
    data = removeUnityTagAlias(filepath)
    nodes = list()

    for data in yaml.load_all(data):
        nodes.append(data)
    
    return (nodes)


def checkTransitions(nodes):
    """ checks transitions in Controller """

    idFalling = None
    for node in nodes:
        if 'AnimatorState' in node.keys() and node['AnimatorState']['m_Name'] == 'Falling':
            idFalling = node['ID']

    idRunning = None
    for node in nodes:
        if 'AnimatorState' in node.keys() and node['AnimatorState']['m_Name'] == 'Running':
            idRunning = node['ID']

    idJump = None
    for node in nodes:
        if 'AnimatorState' in node.keys() and node['AnimatorState']['m_Name'] == 'Jump':
            idJump = node['ID']

    flag = 0
    runToFalling = None
    for node in nodes:
        if 'AnimatorStateTransition' in node.keys() and node['AnimatorStateTransition']['m_Name'] == 'RunningToFalling':
            runToFalling = node['ID']
            if 'm_DstState' in node['AnimatorStateTransition']:
                if node['AnimatorStateTransition']['m_DstState']['fileID'] == idFalling:
                    flag = 1
                    print("Transition points to Falling State: OK")

    if flag == 0:
        print("Transition points to Falling State: No")

    flag = 0
    for node in nodes:
        if 'AnimatorState' in node.keys() and node['AnimatorState']['m_Name'] == 'Running':
            if 'm_Transitions' in node['AnimatorState']:
                for item in node['AnimatorState']['m_Transitions']:
                    if item['fileID'] == runToFalling:
                        flag = 1
                        print("Transition exists from Running State: OK")

    if flag == 0:
        print("Transition exists from Running State: No")

    print("--------------")

    flag = 0
    jumpToFalling = None
    for node in nodes:
        if 'AnimatorStateTransition' in node.keys() and node['AnimatorStateTransition']['m_Name'] == 'JumpToFalling':
            jumpToFalling = node['ID']
            if 'm_DstState' in node['AnimatorStateTransition']:
                if node['AnimatorStateTransition']['m_DstState']['fileID'] == idFalling:
                    flag = 1
                    print("Transition points to Falling State: OK")

    if flag == 0:
        print("Transition points to Falling State: No")

    flag = 0
    for node in nodes:
        if 'AnimatorState' in node.keys() and node['AnimatorState']['m_Name'] == 'Jump':
            if 'm_Transitions' in node['AnimatorState']:
                for item in node['AnimatorState']['m_Transitions']:
                    if item['fileID'] == jumpToFalling:
                        flag = 1
                        print("Transition exists from Jump State: OK")

    if flag == 0:
        print("Transition exists from Jump State: No")


if __name__ == "__main__":
    checkTransitions(loadYAML('Assets/Animators/ty.controller'))
